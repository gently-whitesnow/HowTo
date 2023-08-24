import { EntityStatus } from "../../../entities/entityStatus";
import { EntityTagWrapper } from "./EntityTag.styles";

const EntityTag = (props) => {
  const getEntityTag = () => {
    if (props.status === EntityStatus.Moderation) {
      return (
        <>
          <EntityTagWrapper>
            <>{" (на модерации)"}</>
          </EntityTagWrapper>
        </>
      );
    }

    return <></>;
  };

  return getEntityTag();
};

export default EntityTag;
