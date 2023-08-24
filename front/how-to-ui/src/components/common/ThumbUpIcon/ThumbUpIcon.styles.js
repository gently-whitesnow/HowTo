import styled from "styled-components";

export const ThumbUpIconWrapper = styled.button`
  height: ${(props) => props.size};
  width: ${(props) => props.size};
  border: none;
  background-color: transparent;
  vertical-align: bottom;
  transition: 0.1s;
  :hover {
    cursor: pointer;
    
    transform: scale(1.2);
    
    svg{
      fill: ${(props) => props.color};
    }
  }
`;


