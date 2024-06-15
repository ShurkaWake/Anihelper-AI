<script>
import {defineComponent} from "vue";
import axios from "axios";
import store from "@/store";

export default defineComponent({
  data: () => ({
    loading: false,
    denoise: false,
    upscale: 1,
    filter: "None",
  }),

  methods: {
    getFilterNumber(){
      switch (this.filter){
        case "Gray":
          return 1
        case "Sobel":
          return 2
        default:
          return 0
      }
    },

    async downloadVideo() {
      this.loading = true
      const videoId = this.$route.params.id;
      await axios.get('video/' + videoId + "/process", {
        params: {
          videoFilter: this.getFilterNumber(),
          denoise: this.denoise,
          upscale: this.upscale
        },
        responseType: "blob"
      })
          .then(response => {
            console.log(response)
            const href = URL.createObjectURL(response.data);

            // create "a" HTML element with href to file & click
            const link = document.createElement('a');
            link.href = href;
            link.setAttribute('download', 'anihelper.mp4'); //or any other extension
            document.body.appendChild(link);
            link.click();

            // clean up "a" element & remove ObjectURL
            document.body.removeChild(link);
            URL.revokeObjectURL(href);
          })
          .catch(response => {
            console.log("bad")
            store.commit('setErrorMessage', response.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "/video/" + this.$route.params.id)
            store.commit('switchDialog')
          })
      this.loading = false
    },
  }
})
</script>

<template>
  <v-card class="pa-4" width="400">
    <v-checkbox v-model="denoise" label="Denoise"></v-checkbox>
    <v-slider
        v-model="upscale"
        label="Upscale"
        color="deep-purple-darken-1"
        :min="0.1"
        :max="5"
        :step="0.1"
    >
      <template v-slot:append>
        <v-text-field
            v-model="upscale"
            density="compact"
            style="width: 80px"
            type="number"
            :min="0.1"
            :max="5"
            variant="outlined"
            hide-details
        ></v-text-field>
      </template>
    </v-slider>
    <v-select
        label="Filter"
        v-model="filter"
        :items="['None', 'Gray', 'Sobel']"
    ></v-select>

    <v-container
        class="d-flex justify-end">
      <v-btn
          @click="downloadVideo"
          color="deep-purple-darken-1"
          :loading="loading"
          type="submit"
          text="Download">
      </v-btn>
    </v-container>
  </v-card>
</template>

<style scoped>

</style>