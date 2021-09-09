<template>
  <div class="mb-3">
    <div class="p-3">
      <label class="form-label h3">{{object.Stats[statTypeEnum.Title].Data }}</label>
      <textarea class="form-control" v-model="object.Stats[statTypeEnum.Value].Data" rows="9"  placeholder="edit me"></textarea>
    </div>
    <div class="border mb-3">
      <h3 class="text-start p-3">Prikaz</h3>
      <div v-html="object.Stats[statTypeEnum.Value].Data"></div>
    </div>
  </div>
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
export default class InputComponent extends Vue {
  statTypeEnum = StatTypeEnum
  objectTypeEnum = ObjectTypeEnum
  objectType = ObjectType
  object!: ObjectTemplate
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
