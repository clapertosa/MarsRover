import React, { useEffect } from "react";
import styled from "styled-components";
import { ReactComponent as RoverImage } from "../../assets/images/rover.svg";
import { ReactComponent as RockImage } from "../../assets/images/rock.svg";
import { CELL_SIZE, ROVER_SIZE_OFFSET } from "../../constants/grid";
import cardinalPoint from "../../constants/cardinalPoint";

const Container = styled.div`
  position: relative;
  margin: 10px 0px;

  svg {
    width: ${() => CELL_SIZE - ROVER_SIZE_OFFSET + "px"};
    height: ${() => CELL_SIZE - ROVER_SIZE_OFFSET + "px"};
  }
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

const Grid = ({ size, rover, obstacles, obstacle }) => {
  useEffect(() => {
    try {
      const column = document.getElementById(
        `${obstacle?.posX}-${obstacle?.posY}`
      );
      if (obstacle?.message) {
        column.style.background = "red";
      } else {
        column.style.background = "transparent";
      }
    } catch {}
  }, [obstacle]);

  const getRoverRotation = () => {
    switch (rover.direction) {
      case cardinalPoint.NORTH:
        return -90;
      case cardinalPoint.EAST:
        return 0;
      case cardinalPoint.SOUTH:
        return 90;
      case cardinalPoint.WEST:
        return 180;
      default:
        return 0;
    }
  };

  const getColumns = (rowId) => {
    const columns = [];
    for (let i = 0; i < size; i++) {
      columns.push(<TableColumn key={i} id={`${i}-${rowId}`}></TableColumn>);
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
            top: CELL_SIZE * rover?.posY + ROVER_SIZE_OFFSET / 2 + rover?.posY,
            left: CELL_SIZE * rover?.posX + ROVER_SIZE_OFFSET / 2 + rover?.posX,
            transform: `rotateZ(${getRoverRotation()}deg)`,
          }}
        ></RoverImage>
        {obstacles?.map((obstacle) => (
          <RockImage
            key={obstacle.id}
            style={{
              position: "absolute",
              top:
                CELL_SIZE * obstacle?.posY +
                ROVER_SIZE_OFFSET / 2 +
                obstacle?.posY,
              left:
                CELL_SIZE * obstacle?.posX +
                ROVER_SIZE_OFFSET / 2 +
                obstacle?.posX,
            }}
          ></RockImage>
        ))}
      </Container>
    );
  };

  return createGrid();
};

export default Grid;
