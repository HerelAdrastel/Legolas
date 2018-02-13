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
	public override void Start () {
		_interval = new Interval(SpawnEnemy, Interval);
		
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
		_interval.Update(Time.deltaTime);
	}

	public override void OnMenu() {
		_interval.Pause();
	}

	public override void OnPlay(bool setToPlay) {
		_interval.Play();
	}

	public override void OnGameOver(bool setToGameOver) {
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
