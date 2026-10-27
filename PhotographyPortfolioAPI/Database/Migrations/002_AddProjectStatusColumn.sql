-- SQL Migration to add project status tracking column to project_master

ALTER TABLE project_master ADD COLUMN IF NOT EXISTS status VARCHAR(30) NOT NULL DEFAULT 'ACTIVE';

ALTER TABLE project_master ADD CONSTRAINT chk_project_status CHECK (status IN ('ACTIVE', 'COMPLETED', 'ON_HOLD', 'CANCELLED'));

CREATE INDEX IF NOT EXISTS idx_project_master_status ON project_master(status);
