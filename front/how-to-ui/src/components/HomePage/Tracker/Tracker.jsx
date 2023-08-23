import { observer } from "mobx-react-lite";
import {
  TrackerWrapper,
  TrackerBody,
  LeftSide,
  RightSide,
  Description,
  Title,
  RightUpSide,
  RightBottomSide,
  LeftSideImage,
  ContinueButton,
  ImageWrapper,
} from "./Tracker.styles";

import { useNavigate } from "react-router";
import ProgressBar from "../ProgressBar/ProgressBar";
import { useStore } from "../../../store";
import EntityTag from "../../common/EntityTag/EntityTag";

const Tracker = () => {
  const { colorStore, summaryStore } = useStore();
  const { currentColorTheme } = colorStore;
  const { summaryData } = summaryStore;

  const navigate = useNavigate();
  const onClickHandler = () => {
    navigate(`/${summaryData?.lastCourse?.id}`);
  };

  let percent =
    summaryData?.lastCourse?.articlesCount == 0
      ? 0
      : Math.round(
          (summaryData?.lastCourse?.userApprovedViews /
            summaryData?.lastCourse?.articlesCount) *
            100
        );

  return (
    <TrackerWrapper>
      <TrackerBody onClick={onClickHandler}>
        <LeftSide>
          <LeftSideImage color={currentColorTheme}>
            <ImageWrapper>
              {summaryData?.lastCourse?.image ? (
                <img src={summaryData?.lastCourse?.image}></img>
              ) : null}
            </ImageWrapper>
          </LeftSideImage>
        </LeftSide>
        <RightSide>
          <RightUpSide>
            <Title color={currentColorTheme}>
              {summaryData?.lastCourse?.title}
              <EntityTag status={summaryData?.lastCourse?.status} />
            </Title>
            <Description>{summaryData?.lastCourse?.description}</Description>
          </RightUpSide>
          <RightBottomSide>
            <ContinueButton
              color={currentColorTheme}
              className="continue-button"
            >
              Продолжить
            </ContinueButton>
            <ProgressBar percents={percent} color={currentColorTheme} />
          </RightBottomSide>
        </RightSide>
      </TrackerBody>
    </TrackerWrapper>
  );
};

export default observer(Tracker);
