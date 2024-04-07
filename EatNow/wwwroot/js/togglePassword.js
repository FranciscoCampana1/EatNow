function togglePassword(selectorToggle, selectorPassword) {
    var togglePassword = document.querySelector(selectorToggle);
    var password = document.querySelector(selectorPassword);

    togglePassword.addEventListener("click", () => {
        // Toggle the type attribute using getAttribute() method
        const type = password.getAttribute("type") === "password" ? "text" : "password";
        password.setAttribute("type", type);

        // Toggle the eye and bi-eye icon
        togglePassword.classList.toggle("bi-eye");
    });
}