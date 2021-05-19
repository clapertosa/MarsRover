import { useEffect, useState } from "react";
import styled from "styled-components";
import axiosInstance from "./axiosInstance";
import Grid from "./components/Grid/Grid";
import cardinalPoint from "./constants/cardinalPoint";

const Container = styled.div`
  display: flex;
  flex-flow: column;
`;

const Dropdown = styled.select`
  margin-bottom: 10px;
`;

const App = () => {
  const [planets, setPlanets] = useState([]);
  const [currentPlanet, setCurrentPlanet] = useState(null);
  const [obstacles, setObstacles] = useState([]);
  const [roverPos, setRoverPos] = useState({ x: 0, y: 0 });
  const [roverDirection, setRoverDirection] = useState(cardinalPoint.E);

  const getAllPlanets = () => {
    axiosInstance
      .get("https://localhost:5001/api/planet/all")
      .then((res) => {
        setPlanets([...res.data]);
        setCurrentPlanet(res.data?.[0]);
      })
      .catch(() => {});
  };

  const getAllObstacles = () => {
    axiosInstance
      .get("https://localhost:5001/api/obstacle/all")
      .then((res) => setObstacles([...res.data]))
      .catch(() => {});
  };
  console.log(obstacles);
  useEffect(() => {
    getAllPlanets();
    getAllObstacles();
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
        pos={roverPos}
        direction={roverDirection}
      ></Grid>
    </Container>
  );
};

export default App;
