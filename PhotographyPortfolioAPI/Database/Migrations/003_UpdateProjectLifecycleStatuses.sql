-- Replace the original ACTIVE lifecycle value with the project workflow statuses.

ALTER TABLE project_master DROP CONSTRAINT IF EXISTS chk_project_status;

UPDATE project_master
SET status = 'NEW'
WHERE status = 'ACTIVE' OR status IS NULL;

ALTER TABLE project_master
    ALTER COLUMN status SET DEFAULT 'NEW';

ALTER TABLE project_master
    ADD CONSTRAINT chk_project_status
    CHECK (status IN ('NEW', 'IN_PROGRESS', 'ON_HOLD', 'COMPLETED', 'CANCELLED'));