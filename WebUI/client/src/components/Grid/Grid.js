import React, { useEffect } from "react";
import styled from "styled-components";
import { ReactComponent as RoverImage } from "../../assets/images/rover.svg";
import { CELL_SIZE, ROVER_SIZE_OFFSET } from "../../constants/grid";

const Container = styled.div`
  position: relative;
`;

const Table = styled.table`
  border: 1px solid black;
  padding: 0px;
  margin: 0px;
  border-collapse: collapse;
`;

const TableRow = styled.tr`
  border: 1px solid black;
  padding: 0px;
  margin: 0px;
`;

const TableColumn = styled.td`
  justify-content: center;
  align-items: center;
  border: 1px solid black;
  padding: 0px;
  margin: 0px;
  width: ${() => `${CELL_SIZE}px`};
  height: ${() => `${CELL_SIZE}px`};
`;

const Grid = ({ size, pos, direction }) => {
  const updateRoverPosition = () => {};

  useEffect(() => {
    updateRoverPosition();
  }, [pos]);

  const getColumns = (rowId) => {
    const columns = [];
    for (let i = 0; i < size; i++) {
      columns.push(<TableColumn key={i} id={`${rowId}-${i}`}></TableColumn>);
    }
    return columns;
  };

  const createGrid = () => {
    const rows = [];

    for (let i = 0; i < size; i++) {
      rows.push(
        <TableRow key={i} id={i}>
          {getColumns(i)}
        </TableRow>
      );
    }
    return (
      <Container>
        <Table>
          <tbody>{rows}</tbody>
        </Table>
        <RoverImage
          style={{
            position: "absolute",
            width: CELL_SIZE - ROVER_SIZE_OFFSET,
            height: CELL_SIZE - ROVER_SIZE_OFFSET,
            top: CELL_SIZE * pos.y + ROVER_SIZE_OFFSET / 2 + pos.y,
            left: CELL_SIZE * pos.x + ROVER_SIZE_OFFSET / 2 + pos.x,
          }}
        ></RoverImage>
      </Container>
    );
  };

  return createGrid();
};

export default Grid;
