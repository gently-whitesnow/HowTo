import styled from "styled-components";

export const ComponentWrapper = styled.div`
  width: 100%;
  margin-bottom: 20px;

  hr {
    padding: 0px;
    margin: 0px;
  }
`;

export const InteractiveLabel = styled.div`
  width: 100%;
  text-align: end;
  font-size: 14px;
  color: gray;
`;

export const Border = styled.div`
  min-height: 100px;
  transition: 0.3s;
  border: solid 0.5px;
  border-radius: 4px;
`;

export const HeadContent = styled.div`
  justify-content: space-between;
  display: flex;
`;

export const InteractiveDescription = styled.div`
  font-size: 20px;
  margin-bottom: 20px;
  padding: 20px 20px 15px 20px;
`;

export const InteractiveTextareaDescription = styled.div`
  padding: 20px;
  font-size: 20px;
  margin-bottom: 20px;
`;

export const InteractiveComponentWrapper = styled.div`
  padding-bottom: 20px;
`;

export const SaveAnswerButton = styled.button`
  margin: 0px 20px 20px 20px;
  height: 30px;
  width: 160px;
  border: none;
  background-color: ${(props) => props.color};
  color: white;
  cursor: pointer;
`;

export const SaveAnswerButtonWrapper = styled.div`
  width: 100%;
  display: flex;
  justify-content: flex-end;
`;
