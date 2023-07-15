import { useEffect, useState } from "react";
import { EditWidgetWrapper, IconButtonsWrapper } from "./EditWidget.styles";
import theme from "../../../theme";
import IconButton from "../../common/IconButton/IconButton";
import { ReactComponent as IconEdit } from "../../../icons/pen-edit16.svg";
import { ReactComponent as IconCheck } from "../../../icons/check16.svg";
import { ReactComponent as IconTrash } from "../../../icons/trash16.svg";

const EditWidget = (props) => {
  return (
    <EditWidgetWrapper>
      {props.isEditing ? (
        <>
          <IconButtonsWrapper>
            <IconButton
              color={theme.colors.green}
              onClick={props.onSaveClickHandler}
              active
              size={"30px"}
              disabled={props.isLoading}
            >
              <IconCheck />
            </IconButton>
            <IconButton
              color={theme.colors.red}
              onClick={props.onDeleteClickHandler}
              active
              size={"30px"}
              disabled={props.isLoading}
            >
              <IconTrash />
            </IconButton>
          </IconButtonsWrapper>
          <IconButtonsWrapper>
            <IconButton
              color={props.color}
              onClick={props.onEditClickHandler}
              size={"30px"}
              disabled={props.isLoading}
            >
              <IconEdit />
            </IconButton>
          </IconButtonsWrapper>
        </>
      ) : (
        <IconButtonsWrapper>
          <IconButton
            color={props.color}
            onClick={props.onEditClickHandler}
            size={"30px"}
            disabled={props.isLoading}
          >
            <IconEdit />
          </IconButton>
        </IconButtonsWrapper>
      )}
    </EditWidgetWrapper>
  );
};

export default EditWidget;
