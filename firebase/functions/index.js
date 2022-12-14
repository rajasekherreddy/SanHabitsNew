// The Cloud Functions for Firebase SDK to create Cloud Functions and setup triggers.
const functions = require('firebase-functions');
const nodemailer = require('nodemailer');
var fs = require('fs');

// The Firebase Admin SDK to access Cloud Firestore.
const admin = require('firebase-admin');
admin.initializeApp();

const gmailEmail = 'buildhappinessapp@gmail.com';
const gmailPassword = 'Fbkqhnsebclbkfzp';
const mailTransport = nodemailer.createTransport({
  service: 'gmail',
  auth: {
    user: gmailEmail,
    pass: gmailPassword,
  },
});

async function sendEmailWithRegistrationData(val, template, subject, bodyMessage){
  const mailOptions = {
    from: '"Build Happiness" <noreply@firebase.com>',
    to: val.Email,
    bcc: ["ullaasmenon@gmail.com", "prabakarinfo@gmail.com"],
  };

  var template = fs.readFileSync(template,{encoding:'utf-8'});

  // Building Email message.
  mailOptions.subject = subject;
  mailOptions.html = replaceValuesInHTMLTemplate(template, val, bodyMessage);

  try {
    await mailTransport.sendMail(mailOptions);
    console.log(`Sent email to:`, val.Email);
  } catch(error) {
    console.error('There was an error while sending the email:', error);
  }
  return null;
}

function replaceValuesInHTMLTemplate(template, data, bodyMessage) {
  template = template.replace("{full_name}", data.Name);
  template = template.replace("{full_name}", data.Name);
  template = template.replace("{registration_type}", data.Type);
  template = template.replace("{therapist_category}", data.TherapistCategory);
  template = template.replace("{insurance_accepted}", data.Insurance);
  template = template.replace("{gender}", data.Gender);
  template = template.replace("{support_video_chat}", data.VideoChat);
  template = template.replace("{country}", data.Country);
  template = template.replace("{postal_code}", data.PostalCode);
  template = template.replace("{state}", data.State);
  template = template.replace("{city}", data.City);
  template = template.replace("{Address}", data.Address);
  template = template.replace("{Specialities}", data.Specialities);
  template = template.replace("{language}", data.Language);
  template = template.replace("{business_number}", data.BusinessNumber);
  template = template.replace("{mobile_number}", data.MobileNumber);
  template = template.replace("{website}", data.Website);
  template = template.replace("{fax}", data.Fax);
  template = template.replace("{admin_email}", "info@buildhappiness.app");
  template = template.replace("{message}", bodyMessage);
  return template;
}

exports.sendEmailForServiceRequestRegistration = functions.database.ref('/sprequest/{username}').onCreate(async (snap, context) => {
  sendEmailWithRegistrationData(snap.val(), "new_request.html", "Your request has be successfully submitted!");
});

exports.sendEmailForServiceRequestUpdate = functions.database.ref('/sprequest/{username}').onUpdate(async (change) => {
  sendEmailWithRegistrationData(change.after.val(), "new_request.html", "Your request has be successfully submitted!");
});

exports.sendEmailForAcceptServiceProviderUpdate = functions.database.ref('/approvedsp/{username}').onUpdate(async (change) => {
  sendEmailWithRegistrationData(change.after.val(), "status_change.html", "Congratulations! Your request has been approved!",
  "Your service provider request from Build Happiness app has been approved by the admin, now the users can see the updated data.");
});

exports.sendEmailForAcceptServiceProviderCreate = functions.database.ref('/approvedsp/{username}').onCreate(async (snap, context) => {
  sendEmailWithRegistrationData(snap.val(), "status_change.html", "Congratulations! Your request has been approved!", 
  "Your service provider request from Build Happiness app has been approved by the admin, now the users can choose you as their service provider.");
});

exports.sendEmailForDeclineServiceProviderCreate = functions.database.ref('/rejectedsp/{username}').onCreate(async (snap, context) => {
  sendEmailWithRegistrationData(snap.val(), "status_change.html", "Sorry! Your request has been declined!", 
  "Your service provider request from Build Happiness app has been reviewed and declined by the admin. Reason: " + snap.val().Comments + ". Please create a new request with correct details.");
});

exports.sendEmailForDeclineServiceProviderUpdate = functions.database.ref('/rejectedsp/{username}').onCreate(async (change) => {
  sendEmailWithRegistrationData(change.after.val(), "status_change.html", "Sorry! Your request has been declined!", 
  "Your service provider request from Build Happiness app has been reviewed and declined by the admin.\nReason: " + change.after.val().Comments + ".\nPlease create a new request with correct details.");
});