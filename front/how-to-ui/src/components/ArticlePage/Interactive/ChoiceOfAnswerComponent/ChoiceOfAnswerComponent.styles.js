import styled from "styled-components";

export const ChoiceOfAnswerComponentWrapper = styled.div``;

export const Checkbox = styled.div`
  height: 20px;
  min-width: 20px;
  margin-right: 15px;
  border: solid 0.5px ${(props) => props.color};
  transition: 0.2s;
`;

export const Checkline = styled.div`
  margin-inline: 20px;
  margin-top: 20px;
  display: flex;
  align-items: center;

  cursor: pointer;

  :hover {
    .checkbox {
      background: ${(props) => props.color};
      opacity: 0.4;
    }
    .checked {
      opacity: 1;
    }
  }
  .checked {
    background: ${(props) => props.color};
    opacity: 0.8;
    border: none;
  }
  .true-check {
    background-color: none;
    opacity: 0.8;
    box-shadow: 0px 0px 0px 2px #00d466 inset;
  }
  .false-check {
    background-color: none;
    opacity: 0.8;
    box-shadow: 0px 0px 0px 2px rgba(255, 106, 106, 1) inset;
  }
  .checked.true-check{
    background: #00d466;
  }
  .checked.false-check{
    background: rgba(255, 106, 106, 1);
  }
`;
