﻿using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;
using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Mocks.Entities;
using TrackIt.Queries.Views;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration;

public class ActivityGroupTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private ActivityGroupMock group1 { get; set; }
  
  private ActivityGroupMock group2 { get; set; }
  
  private ActivityGroupMock group3 { get; set; }
  
  [Fact]
  public async Task ShouldCreateAGroup ()
  {
    AddAuthorizationData(PartialSession.Create(await CreateUserWithEmailValidated()));

    var payload = new CreateActivityGroupPayload(
      Title: "Grupo de Tarefas",
      Icon: "icone"
    );
    
    var response = await _httpClient.PostAsync("/activity-group", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var response1 = await _httpClient.GetAsync("/activity-group?page=1&perPage=5");
    var result1 = await response1.ToData<PaginationView<List<ActivityGroupView>>>();
    
    Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
    
    Assert.Equal(1, result1.Page);
    Assert.Equal(1, result1.Pages);
    Assert.Single(result1.Data);
    
    Assert.Equal(1, result1.Data[0].Order);
    Assert.Equal(payload.Title, result1.Data[0].Title);
    Assert.Equal(payload.Icon, result1.Data[0].Icon);
  }

  [Fact]
  public async Task ShouldUpdateAGroup ()
  {
    var user = await CreateUserWithEmailValidated();
    
    AddAuthorizationData(PartialSession.Create(user));

    await CreateActivityGroups(user.Id);
    
    var payload = new UpdateActivityGroupPayload(
      Title: "Diff Grupo de Tarefas",
      Icon: "Diff Icon",
      Order: 2
    );

    var response = await _httpClient.PutAsync($"/activity-group/{group1.Id}", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    
    var response1 = await _httpClient.GetAsync("/activity-group?page=1&perPage=5");
    var result1 = await response1.ToData<PaginationView<List<ActivityGroupView>>>();
    
    Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
    
    Assert.Equal(1, result1.Page);
    Assert.Equal(1, result1.Pages);
    Assert.Equal(3, result1.Data.Count);
    
    foreach (var group in result1.Data)
    {
      if (group.Id != group1.Id) continue;
      
      Assert.Equal(2, group.Order);
      Assert.Equal(payload.Title, group.Title);
      Assert.Equal(payload.Icon, group.Icon);
    }
  }

  [Fact]
  public async Task ShouldDeleteAGroupAndActivities ()
  {
    var user = await CreateUserWithEmailValidated();
    
    AddAuthorizationData(PartialSession.Create(user));

    await CreateActivityGroups(user.Id);
    await CreateActivities(group3.Id);
    
    var response = await _httpClient.DeleteAsync($"/activity-group/{group3.Id}");
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var response1 = await _httpClient.GetAsync("/activity-group?page=1&perPage=5");
    var result1 = await response1.ToData<PaginationView<List<ActivityGroupView>>>();
    
    Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
    
    Assert.Equal(1, result1.Page);
    Assert.Equal(1, result1.Pages);
    Assert.Equal(2, result1.Data.Count);

    List<ActivityGroupMock> groups = [group1, group2];
    
    foreach (var groupData in result1.Data)
    {
      var group = groups.Find(g => g.Id == groupData.Id);
      
      Assert.NotNull(group);
      Assert.Equal(group.Order, groupData.Order);
      Assert.Equal(group.Title, groupData.Title);
      Assert.Equal(group.Icon, groupData.Icon);
    }

    var deletedActivities = await _db.Activity.ToListAsync();
    
    Assert.Empty(deletedActivities);
  }

  [Fact]
  public async Task ShouldThrowActivityGroupNotFound ()
  {
    AddAuthorizationData(PartialSession.Create(await CreateUserWithEmailValidated()));

    var response = await _httpClient.DeleteAsync($"activity-group/{Guid.NewGuid()}");
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Activity group not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  private async Task CreateActivities (Guid groupId)
  {
    var activity1 = new ActivityMock()
      .ChangeTitle("Tarefa 1")
      .ChangeDescription("Descrição para tarefa 1")
      .WithOrder(1)
      .AssignToGroup(groupId);

    var activity2 = new ActivityMock()
      .ChangeTitle("Tarefa 2")
      .ChangeDescription("Descrição para tarefa 2")
      .WithOrder(2)
      .AssignToGroup(groupId);
    
    _db.Activity.AddRange([activity1, activity2]);

    await _db.SaveChangesAsync();
  }
  
  private async Task CreateActivityGroups (Guid userId)
  {
    group1 = new ActivityGroupMock()
      .ChangeTitle("Grupo de Tarefas 1")
      .ChangeIcon("Icon 1")
      .WithOrder(1)
      .AssignToUser(userId);
    
    group2 = new ActivityGroupMock()
      .ChangeTitle("Grupo de Tarefas 2")
      .ChangeIcon("Icon 2")
      .WithOrder(2)
      .AssignToUser(userId);
    
    group3 = new ActivityGroupMock()
      .ChangeTitle("Grupo de Tarefas 3")
      .ChangeIcon("Icon 3")
      .WithOrder(3)
      .AssignToUser(userId);

    _db.ActivityGroup.AddRange([group1, group2, group3]);
    await _db.SaveChangesAsync();
  }
}