<script>
import {defineComponent} from "vue";
import axios from "axios";
import store from "@/store";

export default defineComponent({
  data: () => ({
    prompt: "",
    tittle: "",
    height: 256,
    width: 256,
    seconds: 2,
    fps: 5,
    loading: false,

    video: {}
  }),

  methods: {
    async updateVideo() {
      this.loading = true
      await axios.put('video/' + this.$route.params.id, {
        prompt: this.prompt,
        height: this.height,
        width: this.width,
        seconds: this.seconds,
        fps: this.fps
      })
          .then(response => {
            store.commit('setErrorMessage', "The video has been successfully submitted for processing. It will be available for download soon")
            store.commit('setRedirectPath', "/video")
            store.commit('switchDialog')
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "")
            store.commit('switchDialog')
          })

      this.loading = false
    },

    async fetchVideo() {
      const videoId = this.$route.params.id;
      await axios.get('video/' + videoId)
          .then(response => {
            this.video = response.data.data
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "/video/" + this.$route.params.id)
            store.commit('switchDialog')
          })
    },
  },

  async mounted(){
    await this.fetchVideo()
    this.prompt = this.video.prompt
    this.tittle = this.video.tittle
    this.seconds = this.video.seconds
    this.width = this.video.width
    this.height = this.video.height
    this.fps = this.video.fps
  }
})
</script>

<template>
  <v-card class="pa-4" width="400">
    <v-form
        fast-fail
        @submit.prevent
    >
      <h1 id="form-title">{{ this.tittle }}</h1>
      <v-textarea
          v-model="prompt"
          color="deep-purple-darken-1"
          label="Prompt"
          required>
      </v-textarea>
      <v-row justify="space-between">
        <v-col cols="6">
          <v-text-field
              label="Height"
              type="number"
              :min="128"
              :max="512"
              v-model="height"
              suffix="px"
          ></v-text-field>
        </v-col>

        <v-col cols="6">
          <v-text-field
              label="Width"
              v-model="width"
              type="number"
              :min="128"
              :max="512"
              suffix="px"
          ></v-text-field>
        </v-col>
      </v-row>
      <v-slider
          v-model="seconds"
          label="Seconds"
          color="deep-purple-darken-1"
          :min="2"
          :max="5"
          :step="1"
      >
        <template v-slot:append>
          <v-text-field
              v-model="seconds"
              density="compact"
              style="width: 80px"
              type="number"
              :min="2"
              :max="5"
              variant="outlined"
              hide-details
          ></v-text-field>
        </template>
      </v-slider>

      <v-slider
          v-model="fps"
          label="FPS"
          color="deep-purple-darken-1"
          :step="1"
          :min="5"
          :max="10"
      >
        <template v-slot:append>
          <v-text-field
              v-model="fps"
              density="compact"
              style="width: 80px"
              type="number"
              :min="5"
              :max="10"
              variant="outlined"
              hide-details
          ></v-text-field>
        </template>
      </v-slider>


      <v-container
          class="d-flex justify-end">
        <v-btn
            :loading="loading"
            @click="updateVideo"
            color="deep-purple-darken-1"
            type="submit"
            text="Edit">
        </v-btn>
      </v-container>
    </v-form>
  </v-card>
</template>

<style scoped>
#form-title {
  width: inherit;
  padding: 8px;
  text-align: center;
}
</style>