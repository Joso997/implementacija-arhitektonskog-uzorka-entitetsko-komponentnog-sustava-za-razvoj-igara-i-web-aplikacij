<template>
  <div class="container">
    <ShowComponent msg="Welcome to Your Vue.js + TypeScript App"/>
  </div>
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component'
import ShowComponent from '@/components/ShowComponent.vue' // @ is an alias to /src

@Options({
  components: {
    ShowComponent
  }
})
export default class About extends Vue {}
</script>
