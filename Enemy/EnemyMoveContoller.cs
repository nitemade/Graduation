using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(MoveController))]
public class EnemyMoveController : MonoBehaviour
{
    #region Components
    private Seeker seeker;
    private Transform target;
    private Path path;
    private CharacterStats stats;
    private MoveController moveController;
    #endregion

    #region Path Settings
    [Header("Path Settings")]
    [SerializeField] private float repathRate = 0.5f;     // ÖØÑ°Â·¼ä¸ô
    [SerializeField] private float nextWaypointDistance = 0.2f;
    [SerializeField] private float stopDistance ;   // Í£Ö¹¾àÀë£¨¹¥»÷¾àÀë£©
    #endregion



    #region Runtime
    private int currentWaypoint;
    private float repathTimer = 0f;
    #endregion

    #region Unity

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        stats = GetComponent<CharacterStats>();
        moveController = GetComponent<MoveController>();
    }

    private void Start()
    {
        stopDistance = stats.NormalAttackRange;
    }

    private void Update()
    {
        if (stats.CurrentState != CharacterState.Walk || target == null)
            return;

        HandleRepath();
        MoveAlongPath();
    }

    #endregion

    #region Public API

    /// <summary>
    /// ¿ªÊ¼×·×ÙÄ¿±ê
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        if (target == newTarget)
            return;
        target = newTarget;
        ForceRepath();
    }

    /// <summary>
    /// Í£Ö¹/¿ªÆôÒÆ¶¯
    /// </summary>
    public void StopMove()
    {
        moveController.SetInput(Vector2.zero);
        path = null;
    }


    #endregion

    #region Path Logic

    private void HandleRepath()
    {
        repathTimer += Time.deltaTime;

        if (repathTimer >= repathRate)
        {
            ForceRepath();
            repathTimer = 0f;
        }
    }

    private void ForceRepath()
    {
        if (target == null || seeker == null)
            return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance < stopDistance)
        {
            moveController.SetInput(Vector2.zero);
            return;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (p.error) return;

        path = p;
        currentWaypoint = 0;
    }

    #endregion

    #region Movement

    private void MoveAlongPath()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            moveController.SetInput(Vector2.zero);
            return; 
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // µ½´ïÍ£Ö¹¾àÀë
        if (distanceToTarget <= stopDistance)
        {
            return;
        }

        Vector2 direction = 
            ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;

        moveController.SetInput(direction);

        CheckWaypointDistance();
    }

    private void CheckWaypointDistance()
    {
        if (currentWaypoint >= path.vectorPath.Count)
            return;

        float distance =
            Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    #endregion
}