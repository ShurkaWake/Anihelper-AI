<script>
import {defineComponent} from "vue";
import axios from "axios";
import store from "@/store";

export default defineComponent({
  data: () => ({
    categoryId: 0,
    prompt: "",
    tittle: "",
    height: 256,
    width: 256,
    seconds: 2,
    fps: 5,
    loading: false,

    categories: [],
    selectedCategory: 0,
  }),

  methods: {
    async createVideo(){
      this.loading = true
      await axios.post('video', {
        categoryId: this.selectedCategory.id,
        prompt: this.prompt,
        tittle: this.tittle,
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

    async fetchCategories(){
      await axios.get("category/all")
          .then(response => {
            this.categories = response.data.data
          })
          .catch(error => {
            store.commit('setErrorMessage', error.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "")
            store.commit('switchDialog')
          })
    },

    itemProps (item) {
      return {
        title: item.name,
      }
    },
  },

  async mounted() {
    await this.fetchCategories()
  }
})
</script>

<template>
  <v-card class="pa-4" width="400">
    <v-form
        fast-fail
        @submit.prevent
    >
      <h1 id="form-title">Create Video</h1>
      <v-text-field
          v-model="tittle"
          color="deep-purple-darken-1"
          label="Tittle"
          required>
      </v-text-field>
      <v-textarea
          v-model="prompt"
          color="deep-purple-darken-1"
          label="Prompt"
          required>
      </v-textarea>
      <v-select
        v-model="selectedCategory"
        :items="categories"
        :item-props="itemProps"
        label="Category *"
      />
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
            @click="createVideo"
            color="deep-purple-darken-1"
            type="submit"
            text="Create">
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