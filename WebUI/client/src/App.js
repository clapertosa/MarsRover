import { useEffect, useState } from "react";
import styled from "styled-components";
import axiosInstance from "./axiosInstance";
import Grid from "./components/Grid/Grid";
import cardinalPoint from "./constants/cardinalPoint";
import direction from "./constants/direction";

const Container = styled.div`
  display: flex;
  flex-flow: column;
  align-items: center;

  button {
    height: 100%;
    text-transform: capitalize;
  }
`;

const Dropdown = styled.select``;

const ObstacleError = styled.span`
  color: red;
`;

const App = () => {
  const [planets, setPlanets] = useState([]);
  const [currentPlanet, setCurrentPlanet] = useState(null);
  const [obstacles, setObstacles] = useState([]);
  const [rover, setRover] = useState({
    posX: 0,
    posY: 0,
    direction: cardinalPoint.EAST,
  });
  const [instructions, setInstructions] = useState([]);
  const [obstacle, setObstacle] = useState({
    posX: 0,
    posY: 0,
    message: "",
  });
  const [loading, setLoading] = useState(false);

  const initialize = async () => {
    try {
      setLoading(true);
      // Get Planets
      await axiosInstance.get("/planet/all").then(({ data }) => {
        setPlanets([...data]);
        setCurrentPlanet(data?.[0]);
      });

      // Get Obstacles
      await axiosInstance
        .get("/obstacle/all")
        .then(({ data }) => setObstacles([...data]))
        .catch(() => {});

      // Get Rovers
      await axiosInstance
        .get(`/rover/all`)
        .then(({ data }) => {
          setRover(data[0]);
        })
        .catch(() => {});
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  const moveRover = () => {
    setLoading(true);
    axiosInstance
      .put("/rover/move", {
        id: rover?.id,
        instructions: instructions.map((instruction) => instruction.value),
      })
      .then(({ data }) => {
        setRover((prev) => ({ ...prev, ...data }));
        setLoading(false);
        setInstructions([]);
      })
      .catch(({ response: { data } }) => {
        setInstructions([]);
        setLoading(false);
        const obstaclePosX = data?.obstacle?.PosX;
        const obstaclePosY = data?.obstacle?.PosY;
        setObstacle({
          posX: obstaclePosX,
          posY: obstaclePosY,
          message: `Obstacle at PosX ${obstaclePosX} and PosY ${obstaclePosY}`,
        });
        setRover((prev) => ({
          ...prev,
          posX: data?.rover?.PosX,
          posY: data?.rover?.PosY,
          direction: data?.rover?.Direction,
        }));
      });
  };

  const updateInstructions = (value) => {
    setInstructions((prev) => [...prev, value]);
  };

  useEffect(() => {
    initialize();
  }, []);

  useEffect(() => {
    setObstacle((prev) => ({ ...prev, message: "" }));
  }, [instructions]);

  return (
    <Container>
      <h3 style={{ marginTop: 0 }}>Planets</h3>
      <Dropdown name="Planets">
        {planets.map((planet) => (
          <option key={planet.id} value={planet.id}>
            {planet.name}
          </option>
        ))}
      </Dropdown>
      <Grid
        size={currentPlanet?.size ?? 0}
        rover={rover}
        obstacles={obstacles}
        obstacle={obstacle}
      ></Grid>

      <div style={{ display: "flex", alignItems: "center" }}>
        <button
          type="button"
          disabled={loading}
          style={{ marginRight: 5 }}
          onClick={() => updateInstructions(direction.LEFT)}
        >
          turn left
        </button>
        <div
          style={{
            display: "flex",
            flexFlow: "column",
            justifyContent: "space-between",
          }}
        >
          <button
            type="button"
            disabled={loading}
            onClick={() => updateInstructions(direction.FORWARD)}
          >
            forward
          </button>
          <button
            type="button"
            disabled={loading}
            onClick={() => updateInstructions(direction.BACKWARD)}
          >
            backward
          </button>
        </div>
        <button
          type="button"
          disabled={loading}
          style={{ marginLeft: 5 }}
          onClick={() => updateInstructions(direction.RIGHT)}
        >
          turn right
        </button>
      </div>
      <div style={{ marginTop: 10, marginBottom: 10 }}>
        <button
          type="button"
          disabled={instructions.length <= 0}
          onClick={() => moveRover()}
        >
          Move
        </button>
      </div>
      <ObstacleError>{obstacle?.message}</ObstacleError>
      <div>
        <span>
          {instructions.map((instruction) => instruction.label).join(", ")}
        </span>
      </div>
    </Container>
  );
};

export default App;
