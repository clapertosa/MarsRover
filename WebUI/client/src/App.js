import { useEffect, useState } from "react";
import styled from "styled-components";
import axiosInstance from "./axiosInstance";
import Grid from "./components/Grid/Grid";

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

const App = () => {
  const [planets, setPlanets] = useState([]);
  const [currentPlanet, setCurrentPlanet] = useState(null);
  const [obstacles, setObstacles] = useState([]);
  const [rover, setRover] = useState({ posX: 0, posY: 0 });
  const [loading, setLoading] = useState(false);

  const initialize = () => {
    // Get Planets
    axiosInstance
      .get("/planet/all")
      .then(({ data }) => {
        setPlanets([...data]);
        setCurrentPlanet(data?.[0]);
      })
      .catch(() => {});

    // Get Obstacles
    axiosInstance
      .get("/obstacle/all")
      .then(({ data }) => setObstacles([...data]))
      .catch(() => {});

    axiosInstance
      .get(`/rover?id=1`)
      .then(({ data }) => setRover((prev) => ({ ...prev, ...data })))
      .catch(() => {});
  };

  const moveRover = (direction) => {
    setLoading(true);
    axiosInstance
      .put("/rover/move", { id: rover?.id, direction })
      .then(({ data }) => {
        setRover((prev) => ({ ...prev, ...data }));
        setLoading(false);
      })
      .catch(() => {
        setLoading(false);
      });
  };

  const changeRoverDirection = (direction) => {
    setLoading(true);
    axiosInstance
      .put("/rover/change-direction", { id: rover?.id, direction })
      .then(({ data }) => {
        setRover((prev) => ({ ...prev, ...data }));
        setLoading(false);
      })
      .catch(() => {
        setLoading(false);
      });
  };

  useEffect(() => {
    initialize();
  }, []);

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
      ></Grid>

      <div style={{ display: "flex", alignItems: "center" }}>
        <button
          type="button"
          disabled={loading}
          style={{ marginRight: 5 }}
          onClick={() => changeRoverDirection("l")}
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
            onClick={() => moveRover("f")}
          >
            forward
          </button>
          <button
            type="button"
            disabled={loading}
            onClick={() => moveRover("b")}
          >
            backward
          </button>
        </div>
        <button
          type="button"
          disabled={loading}
          style={{ marginLeft: 5 }}
          onClick={() => changeRoverDirection("r")}
        >
          turn right
        </button>
      </div>
    </Container>
  );
};

export default App;
