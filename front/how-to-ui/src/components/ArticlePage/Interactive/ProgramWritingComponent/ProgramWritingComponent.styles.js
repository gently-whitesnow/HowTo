import styled from "styled-components";

export const ProgramWritingComponentWrapper = styled.div`
  padding: ${(props) => (props.isEditing ? "0px" : "20px")};
  width: ${(props) => (props.isEditing ? "100%" : "auto")};
  textarea {
    padding-left: 10px;
    padding-top: 5px;
  }

  .output-textarea {
    margin-top: 10px;
    border: 1px solid ${(props) => props.textareaColor};
  }
`;

export const SelectorWrapper = styled.div`
  display: flex;
  justify-content: end;
  margin-bottom: 10px;
`;
