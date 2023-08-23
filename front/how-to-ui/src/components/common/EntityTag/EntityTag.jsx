import { EntityStatus } from "../../../entities/entityStatus";
import { EntityTagWrapper } from "./EntityTag.styles";

const EntityTag = (props) => {

  let content = props.status === EntityStatus.Moderation
  ? " (на модерации)"
  : "";

  return (
    <EntityTagWrapper >
      {content}
    </EntityTagWrapper>
  );
};

export default EntityTag;
