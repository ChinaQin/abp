﻿using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging
{
    public class EntityChange : Entity<Guid>, IMultiTenant, IHasExtraProperties
    {
        public virtual Guid AuditLogId { get; protected set; }

        public virtual Guid? TenantId { get; protected set; }

        public virtual DateTime ChangeTime { get; protected set; }

        public virtual EntityChangeType ChangeType { get; protected set; }

        public virtual string EntityId { get; protected set; }

        public virtual string EntityTypeFullName { get; protected set; }

        public ICollection<EntityPropertyChange> PropertyChanges { get; protected set; }

        public virtual Dictionary<string, object> ExtraProperties { get; protected set; }

        protected EntityChange()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public EntityChange(IGuidGenerator guidGenerator, Guid auditLogId, EntityChangeInfo entityChangeInfo)
        {
            Id = guidGenerator.Create();
            AuditLogId = auditLogId;
            TenantId = entityChangeInfo.TenantId;
            ChangeTime = entityChangeInfo.ChangeTime;
            ChangeType = entityChangeInfo.ChangeType;
            EntityId = entityChangeInfo.EntityId;
            EntityTypeFullName = entityChangeInfo.EntityTypeFullName;
            PropertyChanges = entityChangeInfo.PropertyChanges.Select(p => new EntityPropertyChange(guidGenerator, Id, p)).ToList();
            ExtraProperties = entityChangeInfo.ExtraProperties.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
