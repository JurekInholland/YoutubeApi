<script setup lang="ts">
import MenuLink from './MenuLink.vue';
import { Icon } from '@iconify/vue';
</script>

<template>
    <div class="user-menu">
        <div class="header">
            <div class="icon">
                <Icon icon="mdi:user"></Icon>
            </div>
            <div>
                <h2>User Name</h2>
            </div>
        </div>
        <div class="entries">
            <hr>
            <MenuLink to="/user" icon="mdi:account" text="Your channel" />
            <MenuLink to="/user" icon="mdi:account" text="Youtube Studio" />
            <MenuLink to="/user" icon="mdi:account" text="Switch account" />
            <MenuLink to="/user" icon="mdi:account" text="Sign out" />
            <hr>
            <MenuLink to="/user" icon="mdi:account" text="Sign out" />

        </div>
    </div>
</template>

<style scoped lang="scss">
.user-menu {
    width: 300px;
}

.header {
    display: flex;
    flex: 1;
    align-items: center;
    padding: 1rem;
    gap: 1rem;
}

.icon {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    background-color: aqua;
    svg
    {
        width: 100%;
        height: 100%;
    }
}

.entries {
    display: flex;
    flex-direction: column;
    // gap: 0.5rem;
    // padding: 0.5rem;
}
hr {
    border: 0;
    height: 1px;
    background-image: linear-gradient(to right, rgba(255,255,255, 0.15), rgba(255,255,255, 0.25), rgba(255,255,255, 0.15));
    margin: 0.5rem 0;
}
</style>