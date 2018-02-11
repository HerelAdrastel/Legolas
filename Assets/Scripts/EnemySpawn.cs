using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : Game
{

	public Transform Enemy;
	public Transform BigEnemy;
	
	public float Interval;

	private Interval _interval;

	// Use this for initialization
	public void Start () {
		_interval = new Interval(SpawnEnemy, Interval);
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
		_interval.Update(Time.deltaTime);
	}

	public override void onGameOver() {
		_interval.Pause();
	}

	public void SpawnEnemy(int iterations)
	{
		Transform enemy;

		if (iterations > 5 && Random.value < 0.2f)
			enemy = Instantiate(BigEnemy);
		
		else
			enemy = Instantiate(Enemy);

		BoxCollider2D collider = enemy.GetComponent<BoxCollider2D>();

		float halfSize = collider.bounds.size.x / 2;
		
		float x = RandomFloat(Left + halfSize, Right - halfSize);

		enemy.position = new Vector3(x, transform.position.y, transform.position.z);
	}
	
	
}
