using System.Collections.Generic;
using UnityEngine;

public class VRGuide : VRTest
{
    bool fetched;
    List<Quaternion> cachedTurns = new List<Quaternion>();
    Queue<Vector3> visited = new Queue<Vector3>();
    int memory = 10;

    public override void Initialize()
    {
        fetched = false;
        base.Initialize();
    }

    // ================= MOVE =================

    public override Vector3 Move()
    {
        if (cachedTurns.Count > 0)
            return transform.position;

        UpdateMoves();
        fetched = false;

        HashSet<GameObject> inDistControls = new HashSet<GameObject>();

        foreach (var entry in controls)
        {
            ControlInfo info = entry.Value;

            if (info.getTriggered() == 0)
            {
                GameObject obj = info.getObject();
                float distance = Vector3.Distance(transform.position, obj.transform.position);

                if (distance < triggerlimit)
                    inDistControls.Add(obj);
            }
        }

        if (inDistControls.Count == 0)
        {
            Debug.Log("All controls are triggered");
            return transform.position;
        }

        Vector3 best = Vector3.zero;
        float mindiscut = Mathf.Infinity;

        foreach (Vector3 move in moves)
        {
            Vector3 destPos = transform.position + move * moveStep;

            if (Visited(destPos))
                continue;

            float distance = calCutDistance(destPos, inDistControls);

            if (distance < mindiscut)
            {
                best = move;
                mindiscut = distance;
            }
        }

        // fallback aléatoire
        if (best == Vector3.zero)
        {
            int n = Random.Range(0, moves.Count);
            return transform.position + moves[n] * moveStep;
        }

        Vector3 finalDest = transform.position + best * moveStep;

        visited.Enqueue(finalDest);
        if (visited.Count > memory)
            visited.Dequeue();

        return finalDest;
    }

    // ================= TURN =================

    public override Quaternion Turn()
    {
        if (!fetched)
        {
            foreach (var entry in controls)
            {
                ControlInfo control = entry.Value;

                if (control.getTriggered() != 0)
                    continue;

                GameObject obj = control.getObject();
                Vector3 relativePos = obj.transform.position - transform.position;
                float dist = relativePos.magnitude;

                if (dist < triggerlimit && inscope(relativePos.y / dist))
                {
                    if (Physics.Raycast(transform.position, relativePos, out RaycastHit hit, triggerlimit))
                    {
                        if (hit.collider.gameObject == obj)
                        {
                            cachedTurns.Add(
                                Quaternion.LookRotation(relativePos, Vector3.up)
                            );
                        }
                    }
                }
            }

            fetched = true;
        }

        if (cachedTurns.Count > 0)
        {
            Quaternion lookto = cachedTurns[^1];
            cachedTurns.RemoveAt(cachedTurns.Count - 1);
            return lookto;
        }

        return transform.rotation;
    }

    // ================= HELPERS =================

    bool Visited(Vector3 dest)
    {
        foreach (Vector3 v in visited)
        {
            if (Vector3.Distance(v, dest) < 0.01f)
                return true;
        }
        return false;
    }

    public bool inscope(float sin)
    {
        return sin > Mathf.Sin(turnLowerBound.x * Mathf.Deg2Rad)
            && sin < Mathf.Sin(turnUpperBound.x * Mathf.Deg2Rad);
    }

    // ================= CUT DISTANCE =================

    private float calCutDistance(Vector3 movePos, HashSet<GameObject> inDistControls)
    {
        float minCutDist = Mathf.Infinity;

        foreach (GameObject obj in inDistControls)
        {
            float cutDist = calCutDistance(movePos, obj.transform.position);
            if (cutDist < minCutDist)
                minCutDist = cutDist;
        }

        return minCutDist;
    }

    private float calCutDistance(Vector3 movePos, Vector3 targetPos)
    {
        Surface face = acquireFacingSurface(movePos, targetPos);

        if (face == null)
            return 0;

        List<Surface> cuts = face.acquireCuts(targetPos);

        float minCutDist = Mathf.Infinity;

        foreach (Surface cut in cuts)
        {
            float dist = cut.Distance(movePos);
            if (dist < minCutDist)
                minCutDist = dist;
        }

        return minCutDist;
    }

    private Surface acquireFacingSurface(Vector3 movePos, Vector3 targetPos)
    {
        FaceInfo fi = new FaceInfo();

        foreach (var entry in controls)
        {
            GameObject obj = entry.Key;
            Renderer r = obj.GetComponent<Renderer>();

            if (r == null || !r.isVisible)
                continue;

            Vector3 center = r.bounds.center;
            Vector3 extent = r.bounds.extents;

            // (logique inchangée — ton algo original)
            // tu peux garder ton code intersect ici

        }

        return fi.GetFaceSurface();
    }

    // ================= NESTED CLASSES =================

    protected class Surface
    {
        float A, B, C, D;
        float d1h, d1l, d2h, d2l;

        public Surface(float A, float B, float C, float D)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;
        }

        public void SetVertices(float d1h, float d1l, float d2h, float d2l)
        {
            this.d1h = d1h;
            this.d1l = d1l;
            this.d2h = d2h;
            this.d2l = d2l;
        }

        public float Distance(Vector3 point)
        {
            return Mathf.Abs(A * point.x + B * point.y + C * point.z - D)
                   / Mathf.Sqrt(A * A + B * B + C * C);
        }

        public List<Surface> acquireCuts(Vector3 targetPos)
        {
            return new List<Surface>(); // garde ton implémentation si nécessaire
        }
    }

    protected class FaceInfo
    {
        Surface faceSurface;
        float dist = Mathf.Infinity;

        public bool CheckDist(float newdist) => newdist < dist;

        public void Update(Surface face, float dist)
        {
            faceSurface = face;
            this.dist = dist;
        }

        public Surface GetFaceSurface() => faceSurface;
    }
}