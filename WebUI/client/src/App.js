import { useState } from "react";
import Grid from "./components/Grid/Grid";

const App = () => {
  const [roverPos, setRoverPos] = useState({ x: 0, y: 0 });

  return <Grid size={10} pos={roverPos}></Grid>;
};

export default App;
