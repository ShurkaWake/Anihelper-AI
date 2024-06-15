<script>
import {defineComponent} from "vue";

export default defineComponent({
  props: {
    video: {
      type: Object,
      required: true
    }
  }
})
</script>

<template>
  <router-link class="text-decoration-none" :to="'video/' + video.id">
    <v-card
        height="212"
        class="pa-2 text-decoration-none"
        prepend-icon="mdi-video"
        v-bind:title=video.tittle>
      <v-card-text class="overflow-hidden">
        <v-row>
          <v-col style="width: 200px">
            <span v-html="video.prompt.replaceAll('\n', '<br>')" class="pa-2">
            </span>
            <v-divider class="mt-3"/>
            <v-label class="pa-2">Seconds: {{video.seconds}}</v-label>
            <v-divider/>
            <v-label class="pa-2">FPS: {{video.fps}}</v-label>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </router-link>
</template>

<style scoped>

</style>