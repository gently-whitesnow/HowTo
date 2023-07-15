import styled from "styled-components";

export const CheckListComponentWrapper = styled.div``;

// not use checkbox, because it hard customizable
export const Checkbox = styled.div`
  height: 20px;
  min-width: 20px;
  margin-right: 15px;
  border: solid 0.5px ${(props) => props.color};
  transition: 0.2s;
`;

export const Checkline = styled.div`
  width: 100%;
  margin-top: 10px;
  display: flex;
  align-items: center;

  cursor: pointer;

  :hover {
    .checkbox {
      background: ${(props) => props.color};
      opacity: 0.4;
    }
    .right {
      opacity: 1;
    }
  }
  .right {
    background: ${(props) => props.color};
    opacity: 0.8;
  }
`;
