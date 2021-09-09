import { ObjectTemplate } from '../containerClasses/objectTemplate'
import { EventHandlerType } from '../events/types/objectTypes/types'
import { SubObjectTypeEnum } from '../events/types/subObjectType'

export abstract class MechanicAbstract {
  protected abstract SubscribeConditions() : void;
  public abstract UnsubscribeConditions() : any;
  protected abstract Button(eventHandler: EventHandlerType) : void;
  constructor () {
    this.SubscribeConditions()
  }

  public abstract InitGet(_id : number): Promise<ObjectTemplate[]>;

  public abstract InitSet(_objectTemplates: ObjectTemplate[]) : ObjectTemplate[];

  public ObjectTemplates!: ObjectTemplate[];
}
