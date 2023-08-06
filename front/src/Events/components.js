import { IconCheck, IconX } from "@tabler/icons";
import Modal from "../Generic/Modal";
import dayjs from "dayjs";

export function EventExtraMenu({ next, onStatusChange }) {
  if (next.length === 0) {
    return null;
  }

  return next.map((status, index) => {
    switch (status.displayName) {
      case "Approved":
        return (
          <button key={index} className="btn btn-success mx-1" onClick={() => onStatusChange(status)}>
            <IconCheck /> Approve
          </button>
        );
      case "Rejected":
        return (
          <button key={index} className="btn btn-danger mx-1" data-bs-toggle="modal" data-bs-target="#modal-reject">
            <IconX /> Reject
          </button>
        );
      case "Canceled":
        return (
          <button key={index} className="btn btn-warning mx-1" data-bs-toggle="modal" data-bs-target="#modal-cancel">
            <IconX /> Cancel
          </button>
        );
      default:
        return null;
    }
  });
}

export function EventModals({ entity, next, onStatusChange }) {
  if (next.length === 0) {
    return null;
  }

  return next.map((status, index) => {
    switch (status.displayName) {
      case "Approved":
        return null;
      case "Rejected":
        return (
          <Modal
            key={index}
            id="modal-reject"
            title="Are you sure?"
            text={`Do you really want to reject this ${entity.category.displayName} request from ${
              entity.user.displayName
            } starting ${dayjs(entity.startDate).format("DD MMM YYYY")}?`}
            confirm="Reject"
            onConfirm={() => onStatusChange(status)}
          />
        );
      case "Canceled":
        return (
          <Modal
            key={index}
            id="modal-cancel"
            title="Are you sure?"
            text={`Do you really want to cancel this ${entity.category.displayName} request from ${
              entity.user.displayName
            } starting ${dayjs(entity.startDate).format("DD MMM YYYY")}?`}
            confirm="Yes"
            cancel="No"
            onConfirm={() => onStatusChange(status)}
          />
        );
      default:
        return null;
    }
  });
}
