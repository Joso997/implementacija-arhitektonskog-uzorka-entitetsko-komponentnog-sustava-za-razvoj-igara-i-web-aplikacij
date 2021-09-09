<template>
  <tr v-if="showMe" class="p-3 row justify-content-md-center">
    <td colspan="2" class="col-8"><div class="mb-3">
      <div class="p-6">
        <label class="form-label h3">{{object.Stats[statTypeEnum.Title].Data }}</label>
        <textarea class="form-control" :value="object.Stats[statTypeEnum.Value].Data" @change="regionType.RegionTypes[object.Region].ObjectTypes[object.ObjectEnum].ChooseSubType(object, $event.target.value)" rows="9"  placeholder="edit me"></textarea>
      </div>
      <div class="border mb-3">
        <h3 class="text-start p-3">Prikaz</h3>
        <div v-html="object.Stats[statTypeEnum.Value].Data"></div>
      </div>
    </div>
  </td>
    <td class="col-2">
      <component class="ms-3" @click="remove"  v-for="(_objectTemplate, key, index) in objectTemplates" :key="`${ key }-${ index }`" :is="getComponent(_objectTemplate.Region, _objectTemplate.ObjectEnum)" :object='_objectTemplate'> </component>
    </td>
  </tr>

</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component'
import { ObjectTemplate } from '@/interface/manager/containerClasses/objectTemplate'
import { Manager } from '@/interface/manager/mechanics/contentToolkitMechanic'
import { MechanicAbstract } from '@/interface/manager/mechanics/mechanicAbstract'
import { RegionEnum, ObjectTypeEnum, SubObjectTypeEnum, ActionTypeEnum, StatTypeEnum, StatType, ObjectType, RegionType } from '@/interface/manager/events/types/index'
@Options({
  props: {
    object: ObjectTemplate
  }
})
export default class InputComponent extends Vue {
  showMe = true
  statTypeEnum = StatTypeEnum
  objectTypeEnum = ObjectTypeEnum
  objectType = ObjectType
  regionType = RegionType
  regionEnum = RegionEnum
  object!: ObjectTemplate
  mechanic: MechanicAbstract = new Manager.Mechanic.ContentToolkitMechanic()
  objectTemplates = this.mechanic.InitSet(
    [
      new ObjectTemplate(RegionEnum.Form, ObjectTypeEnum.Button, SubObjectTypeEnum.Left, ActionTypeEnum.Click, {
        [StatTypeEnum.Title]: StatType.StatTypes[StatTypeEnum.Title]().CreateStat().InitData('Izbriši'),
        [StatTypeEnum.Value]: this.object.Stats[StatTypeEnum.Value],
        [StatTypeEnum.Design]: StatType.StatTypes[StatTypeEnum.Design]().CreateStat().InitData('btn btn-outline-danger'),
        [StatTypeEnum.Id]: StatType.StatTypes[StatTypeEnum.Id]().CreateStat().InitData(this.object.Stats[StatTypeEnum.Id].Data)
      })
    ]
  )

  beforeUnmount () {
    this.mechanic.UnsubscribeConditions()
  }

  remove () {
    this.showMe = false
  }

  getComponent (_regionEnum : number, _objectEnum: number) {
    return RegionType.RegionTypes[_regionEnum].ObjectTypes[_objectEnum].GetVueComponent()
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
