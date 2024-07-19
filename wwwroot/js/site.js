document.addEventListener("DOMContentLoaded", function () {
    //< !--Resaltar links-- >
    const navLinks = document.querySelectorAll("aside .nav-link") ?? [];
    let shouldUpdateNavState = true;

    navLinks.forEach((navLink) => {
        navLink.addEventListener("click", function () {
            shouldUpdateNavState = false;

            navLinks.forEach((link) => {
                link.classList.remove("active");
            });

            this.classList.add("active");
        });
    });

    const updateNavLinkState = () => {
        if (shouldUpdateNavState) {
            navLinks.forEach((navLink) => {
                navLink.classList.remove("active");
            });

            const nav = [...navLinks].find(
                (el) => el.href === window.location.href
            );

            if (nav) {
                nav.classList.add("active");
            }
        }

        shouldUpdateNavState = true;
    };

    updateNavLinkState();

    window.addEventListener("popstate", updateNavLinkState);

    //< !--Mostrar menu usuario-- >
    const toggleInput = document.getElementById("input-toggleMenu");
    const menuContainer = document.querySelector(".menu-container") ?? [];
    const containerChildren = [...menuContainer.children];

    const handleClickEvent = (e) => {
        const clickedElement = e.target;

        const isClickedElementChildOfMenu = containerChildren.some((el) =>
            el.contains(clickedElement)
        );

        if (!isClickedElementChildOfMenu) {
            toggleInput.checked = false;
            document.removeEventListener("click", handleClickEvent);
        }
    };

    const deployMenuButton = containerChildren.find(
        (el) => el.className === "deployMenu"
    );

    if (deployMenuButton) {
        deployMenuButton.addEventListener("click", () => {
            document.addEventListener("click", handleClickEvent);
        });
    }

    const BtnsideBar = document.getElementById("btn-deploySidebar");
    const sideBar = document.querySelector(".sideBar");
    const gridContainer = document.querySelector(".wrapper");

    BtnsideBar.addEventListener("click", function () {
        sideBar.classList.toggle("aside-small");
        gridContainer.classList.toggle("aside-small");
    });


    //< !--Scroll -->
    const supportsWebkitScrollbar =
        "WebkitAppearance" in document.documentElement.style;
    const containers = document.querySelectorAll(".scroll") ?? [];

    containers.forEach((container) => {
        if (supportsWebkitScrollbar) {
            container.classList.add("custom-scroll");
        } else {
            container.classList.add("native-scroll");
        }
    });
});
