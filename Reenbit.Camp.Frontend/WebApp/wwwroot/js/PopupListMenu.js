let dotNet;
let currentPopupId;

window.attachClosePopup = function(dotNetReference, popupId) {
    dotNet = dotNetReference;
    currentPopupId = popupId;
    document.documentElement.addEventListener("click", checkHidePopup);
}

function checkHidePopup(e) {
    if (!e.target.closest(".list-menu-popup-" + currentPopupId) 
        && !e.target.closest(".menu-button-" + currentPopupId)) 
    {
        document.documentElement.removeEventListener("click", checkHidePopup);
        dotNet.invokeMethodAsync("HideMenu");
    }
}