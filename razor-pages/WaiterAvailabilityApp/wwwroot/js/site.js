// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


/* 
    Schedule page 
*/
const clearPop = document.querySelector(".clear-pop");
const clearBtn = document.querySelector(".clear-btn");

// Hide
clearPop.style.visibility = "hidden";

// Show - for 3s
clearBtn.addEventListener("click", () => {
    clearPop.style.visibility = "visible";

    // setTimeout(() => {
    //     clearPop.style.visibility = "hidden";   
    // }, 3000);
})
/*--------------------------------------------------------*/