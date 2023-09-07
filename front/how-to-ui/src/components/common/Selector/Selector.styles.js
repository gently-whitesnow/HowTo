import styled from "styled-components";

export const SelectorWrapper = styled.div`
  position: relative;
  height: 30px;
  border: 0.2px solid black;
  width: 120px;
  button {
    font-size: 10px;
    border: none;
    width: 100%;
    height: 30px;
    border-radius: 5px;
    :hover {
      background-color: #f0f0f0ff;
    }
  }
  z-index: 10;
  border-radius: 5px;
`;

export const HeaderElement = styled.button`
  cursor: pointer;
  width: 120px;
  font-weight: 600;
  background-color: ${(props) => (props.isOpen ? "#f0f0f0ff" : "white")};
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  svg{
    position: absolute;
    right: 5px;
  }
`;

export const SelectorElementsWrapper = styled.div`
  background-color: white;
  position: absolute;
  right: -5px;
  top: 35px;
  box-shadow: rgba(0, 0, 0, 0.24) 0px 3px 8px;
  min-height: 30px;
  border-radius: 5px;
  width: 130px;
  .selected {
    font-weight: 600;
  }
`;

export const Element = styled.button`
  cursor: pointer;
  border: solid 1px;
  padding: 0px;
  font-weight: 300;
  background-color: white;
`;
