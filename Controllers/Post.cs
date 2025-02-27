﻿using System.Security.Claims;
using CsPostApi.Models.Dto;
using CsPostApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CsPostApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/posts")]
public class PostController(IPostService postService) : ControllerBase
{
    [HttpPost("publish")]
    public async Task<IActionResult> PublishPostAsync([FromBody] PostDto postDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await postService.PublishPostAsync(userId, postDto);
        return Ok();
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdatePostAsync([FromRoute] int id, [FromBody] PostDto postDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await postService.UpdatePostAsync(id, userId, postDto);
        return Ok();
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeletePostAsync([FromRoute] int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await postService.DeletePostAsync(id, userId);
        return Ok();
    }

    [HttpGet("get-by-users/page/{page:int}/pageSize/{pageSize:int}")]
    public async Task<IActionResult> GetPostByUsers([FromRoute] int page, [FromRoute] int pageSize, [FromQuery] IEnumerable<int> ids)
    {
        var result = await postService.GetPostsByUsersAsync(ids, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        var result = await postService.GetPostAsync(id);
        return Ok(result);
    }
}