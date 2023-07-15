import styled from "styled-components";

export const AddInteractiveGridWrapper = styled.div`
  margin-top: 70px;
  margin-bottom: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
`;

export const AddInteractiveButtonLine = styled.div`
  display: flex;
  margin-bottom: 10px;
`;

export const AddInteractiveButton = styled.button`
  margin-inline: 10px;
  height: 50px;
  width: 220px;
  border: none;
  font-size: 18px;
  color: ${(props) => props.color};
  background-color: white;
  border: solid 2px ${(props) => props.color};
  :hover {
    background-color: ${(props) => props.color};
    color: white;
    cursor: pointer;
  }
`;
