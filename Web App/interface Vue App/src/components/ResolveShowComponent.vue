<template>
  <h1 v-if="object.Stats[statTypeEnum.Label].Data === 'Title'">{{object.Stats[statTypeEnum.Value].Data}}</h1>
  <div v-html="object.Stats[statTypeEnum.Value].Data" v-if="object.Stats[statTypeEnum.Label].Data === 'Content'"></div>
  <br v-if="object.Stats[statTypeEnum.Label].Data === 'Content'">
  <div v-if="object.Stats[statTypeEnum.Label].Data === 'Author'">
    <div class="row text-start p-3">
      <figure><blockquote class="blockquote"><p>Autor: {{object.Stats[statTypeEnum.Value].Data}}</p></blockquote>
        </figure>
      </div>
  </div>
  <img :src="object.Stats[statTypeEnum.Value].Data" class="rounded float-start" alt="..." v-if="object.Stats[statTypeEnum.Label].Data === 'Image'">
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component'
import { ObjectTemplate } from '@/interface/manager/containerClasses/objectTemplate'
import { ObjectType, StatTypeEnum, ObjectTypeEnum } from '@/interface/manager/events/types/index'
@Options({
  props: {
    object: ObjectTemplate
  }
})
export default class ResolveShowComponent extends Vue {
  statTypeEnum = StatTypeEnum
  objectTypeEnum = ObjectTypeEnum
  objectType = ObjectType
  object!: ObjectTemplate
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
