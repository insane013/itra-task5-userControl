document.addEventListener("DOMContentLoaded", function () {
    const selectAllCheckbox = document.getElementById("checkAll");
    const checkboxes = document.querySelectorAll(".userCheckbox");

    selectAllCheckbox.addEventListener("change", function () {
        checkboxes.forEach(cb => cb.checked = selectAllCheckbox.checked);
    });

    checkboxes.forEach(cb => cb.addEventListener("change", updateSelectAll));

    function updateSelectAll() {
        const total = checkboxes.length;
        const checked = Array.from(checkboxes).filter(cb => cb.checked).length;

        if (checked === 0) {
            selectAllCheckbox.checked = false;
            selectAllCheckbox.indeterminate = false;
        } else if (checked === total) {
            selectAllCheckbox.checked = true;
            selectAllCheckbox.indeterminate = false;
        } else {
            selectAllCheckbox.checked = false;
            selectAllCheckbox.indeterminate = true;
        }
    }
});
