export function shuffle(arr) {
    var j, temp;
    for (var i = arr.length - 1; i > 0; i--) {
      j = Math.floor(Math.random() * (i + 1));
      temp = arr[j];
      arr[j] = arr[i];
      arr[i] = temp;
    }
    return arr;
  }

  export const BoolArrayChanged = (initialArray, dynamicArray) => {
    for (let index = 0; index < dynamicArray.length; index++) {
      let initialValue = false;
      if (initialArray.length > index)
        initialValue = initialArray[index] ?? false;
      if (initialValue !== (dynamicArray[index] ?? false)) return true;
    }
    return false;
  };

  export const CopyArray = (fromArray, toArray) => {
    for (let i = 0; i < fromArray.length; i++) {
      toArray[i] = fromArray[i]
    }
    return toArray;
  };