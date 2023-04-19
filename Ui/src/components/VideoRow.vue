<script setup lang="ts">
</script>

<template>
    <section class="video-row">
        <ol class="viewport">
            <li>
                <div class="s1">slide1</div>
                <div class="s2">slide2</div>
                <div class="s3">slide3</div>
                <div class="s4">slide4</div>

            </li>
        </ol>
    </section>
</template>

<style scoped lang="scss">
.video-row {
    position: relative;
    width: 100%;
    height: 300px;
    overflow: hidden;
    background-color: blue;

    .viewport {
        position: absolute;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        display: flex;
        overflow-x: scroll;
        counter-reset: item;
        scroll-behavior: smooth;
        scroll-snap-type: x mandatory;
    }
}

ol,
li {
    list-style: none;
    margin: 0;
    padding: 0;
}


.s1 {
    background-color: red;
}

.s2 {
    background-color: green;
}

.s3 {
    background-color: yellow;
}

.s4 {
    background-color: purple;
}


li div {
    position: relative;
    flex: 0 0 100%;
    width: 100%;
    background-color: #f99;
    counter-increment: item;
}
</style>