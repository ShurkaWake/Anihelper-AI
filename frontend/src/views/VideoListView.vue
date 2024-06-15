<script>
import axios from "axios";
import store from "@/store";
import VideoListCard from "@/components/video/VideoListCard.vue";

export default {
  components: {VideoListCard},
  data: () => ({
    videoList: [],
    pages: 1,
    size: 6,
    currentPage: 1,
  }),

  methods: {
    async fetchVideos(){
      await axios.get('video/all', {
        params: {
          page: this.currentPage,
          perPage: this.size
        }
      })
          .then(response => {
            this.videoList = response.data.data.data.value;
            this.pages = response.data.data.pageCount;
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "/")
            store.commit('switchDialog')
          })

    },
  },

  async mounted() {
    await this.fetchVideos()
  }
}
</script>

<template>
  <v-container>
    <v-row justify="center" class="mt-10">
      <v-label
          v-if="videoList === undefined || videoList.length === 0"
          text="There is no songs. Try to create new one"
          class="text-h5">

      </v-label>
    </v-row>
    <v-row>
      <v-col
          v-for="video in videoList"
          :key="video.id"
          cols="6"
      >
        <video-list-card :video="video"/>
      </v-col>
    </v-row>
  </v-container>

  <v-pagination
      :length="pages"
      v-model="currentPage"
      class="mt-6"
      @update:model-value="fetchVideos"></v-pagination>
</template>

<style>

</style>