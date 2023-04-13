<template>
    <div class="error">
        <img src="https://www.youtube.com/img/desktop/unavailable/unavailable_video_dark_theme.png" alt="">

        <h3>This video isn't available any more</h3>

        <router-link to="/">Go to home</router-link>
    </div>
</template>

<style scoped lang="scss">
.error {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    gap: 1rem;
    text-align: center;
    height: 45vh;
    min-height: 480px;

    img {
        max-width: 278px;
    }

    h3 {
        font-size: 1.5rem;
        margin: 2rem 0;
    }

    a {
        font-family: Roboto;
        color: var(--link-color);
        border: 1px solid rgba(255, 255, 255, .25);
        padding: .55rem .75rem;
        border-radius: 1rem;
        text-transform: uppercase;
        font-size: .9rem;
        font-weight: 500;
    }

    a:hover {
        background-color: var(--suggested-action);
        border-color: var(--suggested-action);
    }
}
</style>