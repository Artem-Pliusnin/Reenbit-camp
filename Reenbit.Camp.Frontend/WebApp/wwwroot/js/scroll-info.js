window.getScrollInfo = (element) => {
    const threshold = 150;

    const position = element.scrollTop + element.clientHeight;
    const height = element.scrollHeight;

    return {
        isNearBottom: position >= height - threshold
    };
};