﻿using DreamDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamInfrastructure.EntityConfigurations;
public class InterpretationEntityTypeConfiguration : IEntityTypeConfiguration<Interpretation>
{
    public void Configure(EntityTypeBuilder<Interpretation> builder)
    {
        builder.ToTable("Interpretations");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).IsRequired();
        builder.Property(i => i.ExtractedText).IsRequired();
        builder.Property(i => i.Meaning).IsRequired();
        builder.Property(i => i.Culture).IsRequired();
        builder.Property(i => i.SelectedText.StartOffset).IsRequired();
        builder.Property(i => i.SelectedText.EndOffset).IsRequired();
    }
}