import { useState } from "react";
import {
  Element,
  HeaderElement,
  SelectorElementsWrapper,
  SelectorHeader,
  SelectorWrapper,
} from "./Selector.styles";
import { ReactComponent as IconChevronDown } from "../../../icons/chevron-down.svg";
import { ReactComponent as IconChevronUp } from "../../../icons/chevron-up.svg";

const Selector = (props) => {
  const [isOpen, setIsOpen] = useState(false);
  const [selectedElement, setSelectedElement] = useState(
    props.selectedElement ?? props.elements[0]
  );

  const onClickHandler = (e, element) => {
    e.stopPropagation();
    setSelectedElement(element);
    setIsOpen(!isOpen);
    props.setSelectedElementHandler(element);
  };

  return (
    <SelectorWrapper className="selector">
      <HeaderElement onClick={() => setIsOpen(!isOpen)} isOpen={isOpen}>
        <div>{selectedElement.name}</div>
        {isOpen ? <IconChevronUp /> : <IconChevronDown />}
      </HeaderElement>
      {isOpen ? (
        <SelectorElementsWrapper>
          {props.elements.map((element) => {
            return (
              <Element
                className={
                  selectedElement.name === element.name ? "selected" : ""
                }
                onClick={(e) => onClickHandler(e, element)}
              >
                {element.name}
              </Element>
            );
          })}
        </SelectorElementsWrapper>
      ) : null}
    </SelectorWrapper>
  );
};

export default Selector;
