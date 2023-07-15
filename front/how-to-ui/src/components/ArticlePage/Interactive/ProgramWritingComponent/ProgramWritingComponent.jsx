import { observer } from "mobx-react-lite";
import { ProgramWritingComponentWrapper } from "./ProgramWritingComponent.styles";

const ProgramWritingComponent = (props) => {
  return (
    <ProgramWritingComponentWrapper>ProgramWritingComponent</ProgramWritingComponentWrapper>
  );
};

export default observer(ProgramWritingComponent);
