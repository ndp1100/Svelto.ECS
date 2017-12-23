﻿using System;
using Svelto.ECS.Internal;

namespace Svelto.ECS
{
    public interface IEntityViewBuilder
    {
        IEntityView BuildEntityViewAndAddToList(ref ITypeSafeList list, int entityID);
        ITypeSafeList Preallocate(ref ITypeSafeList list, int size);

        Type GetEntityViewType();
    }

    public class EntityViewBuilder<EntityViewType> : IEntityViewBuilder where EntityViewType : EntityView<EntityViewType>, new()
    {
        public IEntityView BuildEntityViewAndAddToList(ref ITypeSafeList list, int entityID)
        {
            if (list == null)
                list = new TypeSafeFasterListForECSForClasses<EntityViewType>();

            var castedList = list as TypeSafeFasterListForECSForClasses<EntityViewType>;

            var entityView = EntityView<EntityViewType>.BuildEntityView<EntityViewType>(entityID);

            castedList.Add(entityView);

            return entityView;
        }

        public ITypeSafeList Preallocate(ref ITypeSafeList list, int size)
        {
            if (list == null)
                list = new TypeSafeFasterListForECSForClasses<EntityViewType>(size);
            else
                list.ReserveCapacity(size);

            return list;
        }

        public Type GetEntityViewType()
        {
            return _entityViewType;
        }

        readonly Type _entityViewType = typeof(EntityViewType);
    }

    public class EntityStructBuilder<EntityViewType> : IEntityViewBuilder where EntityViewType : struct, IEntityStruct
    {
        public IEntityView BuildEntityViewAndAddToList(ref ITypeSafeList list, int entityID)
        {
            var entityView = default(EntityViewType);
            entityView.ID = entityID;
            
            if (list == null)
                list = new TypeSafeFasterListForECSForStructs<EntityViewType>();

            var castedList = list as TypeSafeFasterListForECSForStructs<EntityViewType>;

            castedList.Add(entityView);

            return null;
        }

        public ITypeSafeList Preallocate(ref ITypeSafeList list, int size)
        {
            if (list == null)
                list = new TypeSafeFasterListForECSForStructs<EntityViewType>(size);
            else
                list.ReserveCapacity(size);

            return list;
        }

        public Type GetEntityViewType()
        {
            return _entityViewType;
        }

        readonly Type _entityViewType = typeof(EntityViewType);
    }    
}