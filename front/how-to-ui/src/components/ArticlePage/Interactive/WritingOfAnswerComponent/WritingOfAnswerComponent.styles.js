import styled from "styled-components";

export const WritingOfAnswerComponentWrapper = styled.div`
    padding: ${(props) => props.isEditing?"0px":"20px"};
    width: ${(props) => props.isEditing?"100%":"auto"};
    textarea{
        padding-left: 10px;
        padding-top: 5px;
        border: 1px solid ${(props) => props.textareaColor};
    }
`;
