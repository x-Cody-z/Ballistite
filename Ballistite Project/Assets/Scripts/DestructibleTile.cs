using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTile : Tile
{
    Projectile projectile;
    //get tile that is within a certain radius of a projectile explosion
    public void GetProjectile(Projectile p)
    {
        projectile = p;
    }

    public void Subscribe()
    {
        projectile.OnProjectileHitTerrain += Projectile_OnProjectileHitTerrain;
    }

    private void Projectile_OnProjectileHitTerrain(object sender, System.EventArgs e)
    {
        //get position of projectile
        //check radius around projectile
        Debug.Log("projectile Hit Terrain");
        projectile.OnProjectileHitTerrain -= Projectile_OnProjectileHitTerrain;
    }

    //update sprite and collision mesh with destruction
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }
}
