using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using test.Models;

namespace test.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("test.Models.Friendship", b =>
                {
                    b.Property<int>("friendshipid")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("friendid");

                    b.Property<int>("userid");

                    b.HasKey("friendshipid");

                    b.HasIndex("friendid");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("test.Models.Invite", b =>
                {
                    b.Property<int>("inviteid")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("senderid");

                    b.Property<int>("userid");

                    b.HasKey("inviteid");

                    b.HasIndex("senderid");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("test.Models.User", b =>
                {
                    b.Property<int>("userid")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("createdat");

                    b.Property<string>("desc");

                    b.Property<string>("email");

                    b.Property<string>("firstname");

                    b.Property<string>("lastname");

                    b.Property<string>("password");

                    b.Property<DateTime>("updatedat");

                    b.HasKey("userid");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("test.Models.Friendship", b =>
                {
                    b.HasOne("test.Models.User", "friend")
                        .WithMany("friends")
                        .HasForeignKey("friendid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("test.Models.Invite", b =>
                {
                    b.HasOne("test.Models.User", "sender")
                        .WithMany("invites")
                        .HasForeignKey("senderid")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
